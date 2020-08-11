import React, { Component } from 'react';
import MessagingContext from '../Context/MessagingContext';
import { withLayoutContext } from '../Context/LayoutContext';
import * as hubConstants from '../Shared/SignalR/HubConstants';
import Subscriber from '../Shared/SignalR/Subscriber';
import axios from 'axios';
import css from './ConversationController.module.css';
import { ConversationApiConstants } from '../Shared/ApiConstants/ApiConstants';
import ConversationView from './ConversationView/ConversationView';
import MessagingView from './MessagingView/MessagingView';

class ConversationController extends Component {
    constructor(props) {
        super(props);
        this.subscriber = Subscriber.getSubscriber(hubConstants.messageHubUrl);
        this.state = {
            conversations: [],
            loadingConversationIds: [],
            activeConversationId: 0,
            loadedAllConversations: false,
            areConversationsLoading: false,
            conversationCountForRequest: 20,
            messageCountForRequest: 20,
            searchResults: []
        };
        this.searchCancelSource = null;
        this.searchTimeout = null;
    }

    componentDidMount = () => {
        this.requestConversations();
        if (this.subscriber.isConnected)
            this.subscriber.connection.on("ReceiveMessage", this.receivedMessage);
        else
            this.subscriber.addOnReadyCallback(() => {
                this.subscriber.connection.on("ReceiveMessage", this.receivedMessage);
            })
    }

    componentDidUpdate = (prevProps, prevState) => {
        if (prevState.conversations.length !== this.state.conversations.length) {
            this.setState(state => {
                let convs = this.sortConversations([...state.conversations]);
                return { conversations: convs };
            })
        }
    }

    requestConversations = olderThan => {
        this.setState({ areConversationsLoading: true });
        let url = `${ConversationApiConstants.getConversations}/${this.state.conversationCountForRequest}`;
        axios.get(url)
            .then(response => {
                switch (response.status) {
                    case 200:
                        let data = response.data;
                        this.appendConversations(data);
                        break;
                    case 204:
                        this.setState({ loadedAllConversations: true });
                        break;
                }
            })
            .catch(err => console.error(err))
            .finally(() => {
                this.setState({ areConversationsLoading: false });
            });
    }

    requestConversation = (id, openWhenReady) => {
        let url = `${ConversationApiConstants.getConversation}/${id}`;
        axios.get(url)
            .then(response => {
                if (response.status === 200) {
                    let conv = response.data;
                    this.prepareConversation(conv);
                    this.setState(state => {
                        let convs = [conv, ...state.conversations];
                        convs = this.sortConversations(convs);
                        return {
                            conversations: convs,
                            activeConversationId: openWhenReady ? conv.id : state.activeConversationId
                        };
                    })
                }
            })
            .catch(err => console.error(err))
            .finally(() => {
                this.setState({ areConversationsLoading: false });
            });
    }

    createConversation = convObject => {
        let url = `${ConversationApiConstants.createConversation}`;
        axios.post(url, convObject)
            .then(response => {
                if (response.status === 201) {
                    let newConv = response.data;
                    this.prepareConversation(newConv);
                    this.setState(state => {
                        return {
                            conversations: [newConv, ...state.conversations],
                            activeConversationId: newConv.id
                        };
                    })
                }
            })
            .catch(err => {
                console.error(err);
            });
    }

    appendConversations = conversations => {
        conversations.forEach(c => this.prepareConversation(c));
        let convs = conversations.concat([...this.state.conversations]);
        let loadedAll = conversations.length < this.state.conversationCountForRequest;
        this.setState({
            conversations: convs,
            loadedAllConversations: loadedAll
        });
    }

    prepareConversation = conv => {
        conv.messages = conv.messages || [];
        conv.messages.forEach(m => m.sendTime = new Date(m.sendTime));
        conv.inputValue = "";
        conv.loadedAny = false;
    }

    requestMessages = (conversationId, olderThan) => {
        this.setState(state => {
            let loading = [...state.loadingConversationIds];
            loading.push(conversationId);
            return { loadingConversationIds: loading };
        });
        let url = `${ConversationApiConstants.getMessages}/${conversationId}/${this.state.messageCountForRequest}`;
        if (olderThan)
            url = `${url}/${olderThan.toJSON()}`;
        axios.get(url)
            .then(response => {
                switch (response.status) {
                    case 200:
                        let data = response.data;
                        this.appendMessages(conversationId, data);
                        break;
                    case 204:
                        this.setConversationReachedEnd(conversationId);
                        break;
                }
            })
            .catch(err => console.error(err))
            .finally(() => {
                this.setState(state => {
                    let loading = [...state.loadingConversationIds];
                    let index = loading.findIndex(l => l === conversationId);
                    if (index !== -1)
                        loading.splice(index, 1);
                    return { loadingConversationIds: loading };
                })
            });
    }

    appendMessages = (conversationId, messages) => {
        messages.forEach(m => m.sendTime = new Date(m.sendTime));
        this.setState(state => {
            let convs = [...state.conversations];
            let index = convs.findIndex(c => c.id === conversationId);
            convs[index].messages = messages.concat(convs[index].messages);
            convs[index].loadedAny = true;
            let loadedAll = messages.length < state.messageCountForRequest;
            if (loadedAll)
                convs[index].reachedEnd = true;
            let loading = [...state.loadingConversationIds];
            index = state.loadingConversationIds.findIndex(id => id === conversationId);
            loading.splice(index, 1);

            return { conversations: convs, loadingConversationIds: loading };
        })
    }

    setConversationReachedEnd = conversationId => {
        this.setState(state => {
            let convs = [...state.conversations];
            let found = convs.find(c => c.id === conversationId);
            if (found) {
                found.reachedEnd = true;
                found.loadedAny = true;
            }
            return { conversations: convs };
        })
    }

    setActiveConversation = id => {
        let isLoaded = this.state.conversations.findIndex(c => c.id === id) !== -1;
        if (isLoaded) {
            this.setState({ activeConversationId: id });
            return;
        }
        this.requestConversation(id, true);
    }

    setInputValue = (conversationId, value) => {
        this.setState(state => {
            let convs = [...state.conversations];
            let conv = convs.find(c => c.id === conversationId);
            if (conv)
                conv.inputValue = value;
            return { conversations: convs };
        })
    }

    exitConversation = () => {
        this.setState({ activeConversationId: 0 });
    }

    sendMessage = messageObj => {
        let url = ConversationApiConstants.sendMessage;
        axios.post(url, messageObj)
            .catch(err => {
                console.error(err);
            });
    }

    receivedMessage = message => {
        message.sendTime = new Date(message.sendTime);
        let unknownMessage = this.state.conversations.findIndex(c => c.id === message.conversationId) === -1;
        if (unknownMessage) {
            this.requestConversation(message.conversationId, false);
            return;
        }
        this.setState(state => {
            let convs = [...state.conversations];
            let conv = convs.find(c => c.id === message.conversationId);
            if (conv)
                conv.messages.splice(conv.messages.length, 0, message);
            convs = this.sortConversations(convs);
            return { conversations: convs };
        })
    }

    sortConversations = convs => {
        const getSendTimes =
            conversation => conversation.messages.map(m => m.sendTime);
        convs = convs.sort((c1, c2) => {
            let val1 = Math.max.apply(null, getSendTimes(c1));
            let val2 = Math.max.apply(null, getSendTimes(c2));
            return val2 - val1;
        });
        return convs;
    }

    searchConversations = searchStr => {
        clearTimeout(this.searchTimeout);

        if (this.searchCancelSource) {
            this.searchCancelSource.cancel();
            this.searchCancelSource = null;
        }

        if (!searchStr) {
            this.setState({ searchResults: [] });
            return;
        }

        this.searchTimeout = setTimeout(() => {
            let url = `${ConversationApiConstants.search}/${searchStr}`;
            if (this.searchCancelSource)
                this.searchCancelSource.cancel();
            this.searchCancelSource = axios.CancelToken.source();
            axios.get(url, { cancelToken: this.searchCancelSource.token })
                .then(response => {
                    if (response.status === 200) {
                        this.setState({ searchResults: response.data });
                    }
                    this.searchCancelSource = null;
                })
                .catch(err => {
                    if (axios.isCancel(err))
                        return;
                    console.error(err)
                });
        }, 400);

    }

    render = () => {
        const wrapperClasses = [css.controllerWrapper];
        if (this.props.layoutContext.messageViewEnabled)
            wrapperClasses.push(css.active);
        const wrapperClasList = wrapperClasses.join(' ');
        let content = <ConversationView />;
        if (this.state.activeConversationId) {
            const isConvLoading = this.state.loadingConversationIds
                .findIndex(c => c === this.state.activeConversationId) !== -1;
            const currentConv = this.state.conversations.find(c => c.id === this.state.activeConversationId);
            content = <MessagingView key={this.state.activeConversationId}
                conversationId={this.state.activeConversationId}
                conversation={currentConv}
                isLoading={isConvLoading} />;
        }
        return (
            <MessagingContext.Provider value={{
                ...this.state,
                enterConversation: this.setActiveConversation,
                exitConversation: this.exitConversation,
                requestMessages: this.requestMessages,
                setConversationInput: this.setInputValue,
                sendMessage: this.sendMessage,
                searchConversations: this.searchConversations,
                createConversation: this.createConversation
            }}>
                <aside>
                    <div className={wrapperClasList}>
                        {content}
                    </div>
                </aside>
            </MessagingContext.Provider>
        );
    }
}

export default withLayoutContext(ConversationController);