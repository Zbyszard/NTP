import React, { Component } from 'react';
import Backdrop from '../../Backdrop/Backdrop';
import PropTypes from 'prop-types';
import HamburgerIcon from '../HamburgerIcon/HamburgerIcon';
import classes from '../Menu.module.css';

class SideMenu extends Component {
    constructor(props) {
        super(props);

    }

    render() {
        let menuClasses = [classes.sideMenu];
        if (!this.props.isActive)
            menuClasses.push(classes.hidden);
        let menuClassList = menuClasses.join(' ');
        return (
            <>
                <div className={menuClassList}>
                    <HamburgerIcon
                        containerClass={classes.menuIcon}
                        isActive={true}
                        clickHandler={this.props.toggleHandler} />
                    <ul className={classes.menuList}>
                        {this.props.children}
                    </ul>
                </div>
                <Backdrop clickHandler={this.props.toggleHandler}
                    isActive={this.props.isActive}
                    zIndex={10} />
            </>
        );
    }
}

SideMenu.propTypes = {
    isActive: PropTypes.bool.isRequired,
    toggleHandler: PropTypes.func
}

export default SideMenu;