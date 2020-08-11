import React, { useContext } from 'react';
import PropTypes from 'prop-types';
import AuthContext from '../../Context/AuthContext';
import MessagingContext from '../../Context/MessagingContext';
import formatDate from '../../Shared/Functions/formatDate';
import css from './Conversation.module.css';

const Conversation = props => {

    const authContext = useContext(AuthContext);
    const messagingContext = useContext(MessagingContext);
    const displaySender = props.lastMessage.sender === authContext.userName ? "You" : props.lastMessage.sender;

    const clickHandler = () => {
        messagingContext.enterConversation(props.id);
    }

    return (
        <li className={css.wrapper} onClick={clickHandler}>
            <div className={css.name}>
                {props.name}
            </div>
            <div className={css.date}>
                {formatDate(props.lastMessage.sendTime)}
            </div>
            <div className={css.text}>
                {`${displaySender}: ${props.lastMessage.text}`}
            </div>
        </li>
    );
}

Conversation.propTypes = {
    id: PropTypes.number.isRequired,
    name: PropTypes.string,
    lastMessage: PropTypes.object
}

export default Conversation;