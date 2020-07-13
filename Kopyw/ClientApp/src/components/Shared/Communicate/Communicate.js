import React from 'react';
import PropTypes from 'prop-types';
import classes from './Communicate.module.css';

const Communicate = props => {
    let position = props.position;
    switch (position) {
        default:
        case "absolute":
            position = classes.absolute;
            break;
        case "fixed": position = classes.fixed;
            break;
    }
    return (
        <div className={`${classes.communicate} ${position}`}
            style={{ zIndex: props.zIndex }}>
            {props.children}
        </div>
    );
}

Communicate.propTypes = {
    zIndex: PropTypes.number,
    position: PropTypes.oneOf(["fixed", "absolute"])
}

export default Communicate;