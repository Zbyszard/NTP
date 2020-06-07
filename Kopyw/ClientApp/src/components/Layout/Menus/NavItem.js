import React from 'react';
import PropTypes from 'prop-types';

const NavItem = props => {

    return (
        <li onClick={props.clickHandler}>
            {props.children}
        </li>
    );
}

NavItem.propTypes = {
    clickHandler: PropTypes.func
}

export default NavItem;