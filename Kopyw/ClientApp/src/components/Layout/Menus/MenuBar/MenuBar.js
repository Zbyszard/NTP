import React from 'react';
import PropTypes from 'prop-types';
import HamburgerIcon from "../HamburgerIcon/HamburgerIcon";
import SearchBar from "./Search";
import classes from '../Menu.module.css';

const MenuBar = props => {
    return (
        <div className={classes.topBar}>
            <HamburgerIcon
                containerClass={classes.menuIcon} isActive={false}
                clickHandler={props.menuIconClickHandler} />
            <SearchBar />
            <ul className={classes.menuList}>
                {props.children}
            </ul>
        </div>
    )
}

MenuBar.propTypes = {
    menuIconClickHandler: PropTypes.func
}

export default MenuBar; 