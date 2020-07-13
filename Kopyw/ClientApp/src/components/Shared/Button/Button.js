import React from 'react';
import PropTypes from 'prop-types';
import classes from './Button.module.css';

const Button = props => {

    const style = { fontSize: props.fontSize || "1rem" };
    return (
        <button onClick={props.onClick}
            className={classes.button}
            style={style}>
            {props.children}
        </button>
    );
}

Button.propTypes = {
    onClick: PropTypes.func,
    fontSize: PropTypes.string
}

export default Button;