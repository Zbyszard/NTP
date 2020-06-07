import React from 'react';
import PropTypes from 'prop-types';
import classes from './Backdrop.module.css';

const Backdrop = props => {

    return (
        <div className={classes.backdrop}
            onClick={props.onclick()}
            style={{zIndex: props.zIndex}} />
    );
}

Backdrop.propTypes = {
    onclick: PropTypes.func,
    zIndex: PropTypes.number
}

export default Backdrop;