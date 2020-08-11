import React, { useState, useEffect, useContext } from 'react';
import PropTypes from 'prop-types';
import HamburgerIcon from "../HamburgerIcon/HamburgerIcon";
import SearchBar from "./Search";
import MenuContext from '../MenuContext';
import LayoutContext from '../../../../Context/LayoutContext';
import css from '../Menu.module.css';
import AuthorizedRender from '../../../api-authorization/AuthorizedRender';
import '../../../Shared/Icons/css/fontello.css';

const MenuBar = props => {
    const menuContext = useContext(MenuContext);
    const layoutContext = useContext(LayoutContext);
    const [iconSize, setIconSize] = useState("1rem");

    useEffect(() => {
        let inputHeight = `${menuContext.searchInputRef.current.clientHeight * 0.8}px`;
        setIconSize(inputHeight);
    }, []);

    const messageIconClickHandler = () => {
        layoutContext.setMessageViewEnabled(!layoutContext.messageViewEnabled);
    }

    let iconContainerClasses = [css.iconContainer];
    if (!menuContext.showSearch)
        iconContainerClasses.push(css.active);
    let iconContainerClassList = iconContainerClasses.join(' ');
    let messageIconCore =
        <>
            {/* <div className={css.newMessages}>1</div> */}
            <i className="icon-comment" />
        </>;
    let messageIconStd =
        <AuthorizedRender>
            <div className={css.messageIconContainer}
                onClick={messageIconClickHandler}>
                {messageIconCore}
            </div>
        </AuthorizedRender>;
    let messageIconMobile =
        <AuthorizedRender>
            <div className={css.messageIconMobileContainer}
                onClick={messageIconClickHandler}>
                {!layoutContext.messageViewEnabled ?
                    messageIconCore : <i className="icon-left-big" />}
            </div>
        </AuthorizedRender>;
    return (
        <div className={css.topBar}>
            <HamburgerIcon isActive={false}
                containerClass={css.menuIcon}
                clickHandler={props.menuIconClickHandler} />
            <SearchBar />
            <div className={iconContainerClassList} style={{ fontSize: iconSize }}>
                <div className={css.searchIconContainer}>
                    <i className="icon-search" onClick={menuContext.searchIconClick} />
                </div>
                {messageIconStd}
                {messageIconMobile}
            </div>
            <ul className={css.menuList}>
                {props.children}
            </ul>
        </div>
    )
}

MenuBar.propTypes = {
    menuIconClickHandler: PropTypes.func
}

export default MenuBar;

