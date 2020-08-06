import React, { useContext } from 'react';
import PropTypes from 'prop-types';
import AuthContext from '../../Context/AuthContext';
import css from './Message.module.css';

const Message = props => {
    const authContext = useContext(AuthContext);
    const classes = [css.message];
    if (props.sender === authContext.userName)
        classes.push(css.toRight);
    else
        classes.push(css.toLeft);
    const classList = classes.join(' ');
    return (
        <div className={classList}>
            {props.children}
        </div>
    );
}

Message.propTypes = {
    sender: PropTypes.string,
    sendTime: PropTypes.instanceOf(Date)
}

export default Message;