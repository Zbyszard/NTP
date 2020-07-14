import React, { useState, useRef } from 'react';
import PropTypes from 'prop-types';
import classes from './ContextMenu.module.css';

const ContextMenu = props => {

    const [active, setActive] = useState(false);
    const [menuXPos, setmenuXPos] = useState(0);
    const [menuYPos, setmenuYPos] = useState(0);
    const menuRef = useRef(null);
    const toggle = e => {
        const clickListener = () => {
            setActive(false);
            document.removeEventListener("click", clickListener);
        };
        setmenuXPos(e.clientX);
        setmenuYPos(e.clientY);
        if (!active) {
            menuRef.current.focus();
            document.addEventListener("click", clickListener);
        }
        setActive(!active);
    }
    const hide = () => {
        setActive(false);
    };

    let menuClassList = [classes.menu];
    if (active)
        menuClassList.push(classes.active);
    const menuClasses = menuClassList.join(' ');
    const style = { top: menuYPos + 10, left: menuXPos, transform: "translateX(-100%)" };
    return (
        <div className={classes.container}>
            <i className={classes.meatballs} onClick={toggle} />
            <div className={menuClasses}
                ref={menuRef}
                onBlur={hide}
                style={style}>
                {props.children}
            </div>
        </div>
    );
}

ContextMenu.propTypes = {
    children: PropTypes.array
}

export default ContextMenu;