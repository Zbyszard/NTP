import React from 'react';
import Backdrop from '../../../Shared/Backdrop/Backdrop';
import PropTypes from 'prop-types';
import HamburgerIcon from '../HamburgerIcon/HamburgerIcon';
import classes from '../Menu.module.css';

const SideMenu = props => {

    let menuClasses = [classes.sideMenu];
    if (!props.isActive)
        menuClasses.push(classes.hidden);
    let menuClassList = menuClasses.join(' ');
    return (
        <>
            <div className={classes.fixedParent}>
                <div className={menuClassList}>
                    <HamburgerIcon
                        containerClass={classes.menuIcon}
                        isActive={true}
                        clickHandler={props.toggleHandler} />
                    <ul className={classes.menuList}>
                        {props.children}
                    </ul>
                </div>
            </div >
            <Backdrop clickHandler={props.toggleHandler}
                isActive={props.isActive}
                zIndex={10} />
        </>
    );
}

SideMenu.propTypes = {
    isActive: PropTypes.bool.isRequired,
    toggleHandler: PropTypes.func
}

export default SideMenu;