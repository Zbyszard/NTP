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
            messageCountForRequest: 20
        };
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

    requestConversations = () => {
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
            .catch(err => console.error(err));
    }

    appendConversations = conversations => {
        conversations.forEach(c => {
            c.messages = c.messages || [];
            c.messages.forEach(m => m.sendTime = new Date(m.sendTime));
            c.inputValue = "";
            c.loadedAny = false;
        });
        let convs = conversations.concat([...this.state.conversations]);
        let loadedAll = conversations.length < this.state.conversationCountForRequest;
        this.setState({
            conversations: convs,
            areConversationsLoading: false,
            loadedAllConversations: loadedAll
        });
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
            }
            )
            .catch(err => console.error(err));
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
            if (found)
                found.reachedEnd = true;
            return { conversations: convs };
        })
    }

    setActiveConversation = id => {
        this.setState({ activeConversationId: id });
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
        convs = convs.sort((c1, c2) => {
            let val1 = Math.max.apply(null, c1.messages.map(m => m.sendTime));
            let val2 = Math.max.apply(null, c2.messages.map(m => m.sendTime));
            return val2 - val1;
        });
        return convs;
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
                sendMessage: this.sendMessage
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