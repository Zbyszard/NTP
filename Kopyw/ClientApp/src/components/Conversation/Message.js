import React, { useContext, useState, useEffect, useRef } from 'react';
import PropTypes from 'prop-types';
import AuthContext from '../../Context/AuthContext';
import css from './Message.module.css';
import formatDate from '../../Shared/Functions/formatDate';

const Message = props => {
    const authContext = useContext(AuthContext);
    const rowRef = useRef();
    const timeRef = useRef();
    const [rowClasses, setRowClasses] = useState([css.messageRow]);
    const senderIsLoggedUserRef = useRef(props.sender === authContext.userName);

    useEffect(() => {
        if (props.showTime)
            setRowClasses([css.messageRow, css.showTime]);
        else
            setRowClasses([css.messageRow]);
    }, [props.showTime]);

    const clickHandler = () => {
        props.clickCallback(props.sendTime);
    }

    if (senderIsLoggedUserRef.current) {
        rowClasses.push(css.toRight);
    }
    else {
        rowClasses.push(css.toLeft);
    }
    const rowClassList = rowClasses.join(' ');
    return (
        <div className={rowClassList} ref={rowRef}>
            <div className={css.time} ref={timeRef}>
                {formatDate(new Date(props.sendTime)).replace(/\s/, '\n')}
            </div>
            <div className={css.message} onClick={clickHandler}>
                {props.children}
            </div>
        </div>
    );
}

Message.propTypes = {
    sender: PropTypes.string,
    sendTime: PropTypes.instanceOf(Date),
    showTime: PropTypes.bool.isRequired,
    clickCallback: PropTypes.func
}

export default Message;