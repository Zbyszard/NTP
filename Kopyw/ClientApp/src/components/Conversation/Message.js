import React, { useContext } from 'react';
import PropTypes from 'prop-types';
import AuthContext from '../../Context/AuthContext';
import css from './Message.module.css';
import formatDate from '../../Shared/Functions/formatDate';

const Message = props => {
    const authContext = useContext(AuthContext);
    const msgClasses = [css.message];

    const clickHandler = () => {
        props.clickCallback(props.sendTime);
    }

    if (props.sender === authContext.userName) {
        msgClasses.push(css.toRight);
        if (props.showTime)
            msgClasses.push(css.translateLeft);
    }
    else {
        msgClasses.push(css.toLeft);
        if (props.showTime)
            msgClasses.push(css.translateRight);
    }
    const msgClassList = msgClasses.join(' ');
    const time = props.showTime ?
        <div className={css.time}>
            {formatDate(props.sendTime).replace(/\s/, '\n')}
        </div> :
        null;
    return (
        <div className={css.messageRow}>
            <div className={msgClassList} onClick={clickHandler}>
                {props.children}
            </div>
            {time}
        </div>
    );
}

Message.propTypes = {
    sender: PropTypes.string,
    sendTime: PropTypes.instanceOf(Date),
    showTime: PropTypes.bool,
    clickCallback: PropTypes.func
}

export default Message;