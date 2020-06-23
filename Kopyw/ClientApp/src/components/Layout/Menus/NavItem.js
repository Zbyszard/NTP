import React, { useContext } from 'react';
import MenuContext from './MenuContext';
import PropTypes from 'prop-types';

const NavItem = props => {
    const context = useContext(MenuContext);
    return (
        <li onClick={context.navItemClicked}>
            {props.children}
        </li>
    );
}

NavItem.propTypes = {
    clickHandler: PropTypes.func
}

export default NavItem;