import React from 'react';
import PropTypes from 'prop-types';
import './HamburgerIcon.css';

const HamburgerIcon = props => {

    let btnClass = "hamburger hamburger--squeeze";
    if (props.isActive)
        btnClass += " is-active"

    return (
        <div className={props.containerClass}>
            <button className={btnClass} type="button"
                onClick={props.clickHandler}>
                <span className="hamburger-box">
                    <span className="hamburger-inner"></span>
                </span>
            </button>
        </div>

    );
}

HamburgerIcon.propTypes = {
    isActive: PropTypes.bool,
    containerClass: PropTypes.string.isRequired,
    clickHandler: PropTypes.func
}

export default HamburgerIcon;