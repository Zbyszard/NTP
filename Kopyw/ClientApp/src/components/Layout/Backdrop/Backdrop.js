import React from 'react';
import PropTypes from 'prop-types';
import classes from './Backdrop.module.css';

const Backdrop = props => {

    let backdropClasses = [classes.backdrop];
    if (!props.isActive)
        backdropClasses.push(classes.hidden);
    let backdropClassList = backdropClasses.join(' ');
    return (
        <div className={backdropClassList}
            onClick={props.clickHandler}
            style={{ zIndex: props.zIndex }} />
    );
}

Backdrop.propTypes = {
    clickHandler: PropTypes.func,
    zIndex: PropTypes.number,
    isActive: PropTypes.bool.isRequired
}

export default Backdrop;