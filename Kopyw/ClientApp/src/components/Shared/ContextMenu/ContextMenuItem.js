import React from 'react';
import PropTypes from 'prop-types';
import classes from './ContextMenu.module.css';

const ContextMenuItem = props => {

    return (
        <div className={classes.item} onClick={props.clickCallback}>
            {props.children}
        </div>
    );
}

ContextMenuItem.propTypes = {
    clickCallback: PropTypes.func,
    children: PropTypes.string
}

export default ContextMenuItem;