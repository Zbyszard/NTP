import React, { useContext } from 'react';
import PropTypes from 'prop-types';
import MessagingContext from '../../Context/MessagingContext';
import css from './ConversationView.module.css';
import Conversation from '../../components/Conversation/Conversation';
import Communicate from '../../components/Shared/Communicate/Communicate';
import LoadingSymbol from '../../components/Shared/LoadingSymbol/LoadingSymbol';
import ConversationSearchBar from './ConversationSearchBar';
import getConversationDisplayName from '../../Shared/Functions/getConversationDisplayName';
import AuthContext from '../../Context/AuthContext';

const ConversationView = props => {
    const messagingContext = useContext(MessagingContext);
    const authContext = useContext(AuthContext);

    let content = null;
    if (messagingContext.areConversationsLoading)
        content = <Communicate zIndex={-1}><LoadingSymbol /></Communicate>;
    else {
        content = messagingContext.conversations.filter(c => c.messages.length > 0).map(c =>{
            let name = getConversationDisplayName(c, authContext.userName);
            return <Conversation id={c.id} key={c.id}
                name={name}
                lastMessage={c.messages[c.messages.length - 1]} />;
        });
        if (content.length === 0)
            content = <Communicate zIndex={-1}>You don't have any conversations</Communicate>;
    }
    return (
        <div className={css.outerContainer}>
            <ConversationSearchBar />
            <ol className={css.conversationList}>
                {content}
            </ol>
        </div>
    );
}

ConversationView.propTypes = {
    stringChangedCallback: PropTypes.func
}

export default ConversationView;