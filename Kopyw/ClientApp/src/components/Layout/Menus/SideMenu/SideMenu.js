import React, { Component } from 'react';
import Backdrop from '../../Backdrop/Backdrop';
import PropTypes from 'prop-types';
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
                    <ul className={classes.menuList}>
                        {this.props.children}
                    </ul>
                </div>
                <Backdrop />
            </>
        );
    }

    close = () => {
        
    }
}

SideMenu.propTypes = {
    isActive: PropTypes.bool.isRequired
}

export default SideMenu;