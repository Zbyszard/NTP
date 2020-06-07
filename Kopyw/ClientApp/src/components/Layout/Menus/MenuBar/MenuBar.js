import React from 'react';
import PropTypes from 'prop-types';
import HamburgerIcon from "../HamburgerIcon/HamburgerIcon";
import classes from '../Menu.module.css';

const MenuBar = props => {
    return (
        <div className={classes.topBar}>
            <HamburgerIcon
                containerClass={classes.menuIcon}
                isActive={props.sideMenuActive} />
            <ul className={classes.menuList}>
                {props.children}
            </ul>
        </div>
    )
}

MenuBar.propTypes = {
    sideMenuActive: PropTypes.bool.isRequired
}

export default MenuBar; 