import React, { useContext } from 'react';
import PropTypes from 'prop-types';
import MessagingContext from '../../Context/MessagingContext';
import css from './ConversationView.module.css';
import Conversation from '../../components/Conversation/Conversation';
import Communicate from '../../components/Shared/Communicate/Communicate';
import LoadingSymbol from '../../components/Shared/LoadingSymbol/LoadingSymbol';

const ConversationView = props => {
    const messagingContext = useContext(MessagingContext);
    let content = null;
    if (messagingContext.areConversationsLoading)
        content = <Communicate><LoadingSymbol /></Communicate>;
    else
        content = messagingContext.conversations &&
            messagingContext.conversations.map(c =>
                <Conversation id={c.id}
                    name={c.name}
                    isGroup={c.isGroup}
                    lastMessage={c.messages[c.messages.length - 1]}
                    users={c.userNames} />) ||
            <Communicate>You don't have any conversations</Communicate>;
    return (
        <div className={css.conversationList}>
            {content}
        </div>
    );
}

export default ConversationView;