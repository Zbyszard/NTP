import React, { useState, useRef } from 'react';
import { Link } from 'react-router-dom';
import { LoginMenu } from '../../api-authorization/LoginMenu';
import MenuBar from './MenuBar/MenuBar';
import SideMenu from './SideMenu/SideMenu';
import NavItem from './NavItem';
import AuthorizedRender from '../../api-authorization/AuthorizedRender';
import MenuContext from './MenuContext';

const NavMenu = () => {
  const [showSearch, setShowSearch] = useState(false);
  const [showSideMenu, setShowSideMenu] = useState(false);
  const searchInputRef = useRef(null);

  const enableSearch = () => {
    setShowSearch(true);
  };

  const disableSearch = () => {
    setShowSearch(false);
  };

  const toggleSideMenu = () => {
    setShowSideMenu(!showSideMenu);
  };

  const closeSideMenu = () => {
    setShowSideMenu(false);
  };

  const searchIconClickHandler = () => {
    enableSearch();
    searchInputRef.current.focus();
}

  const navItems =
    <>
      <NavItem>
        <Link to="/">New posts</Link>
      </NavItem>
      <NavItem>
        <Link to="/top">Top</Link>
      </NavItem>
      <AuthorizedRender>
        <NavItem>
          <Link to="/observed">Observed</Link>
        </NavItem>
        <NavItem>
          <Link to="/me">Your posts</Link>
        </NavItem>
      </AuthorizedRender>
      <LoginMenu />
    </>;
  return (
    <MenuContext.Provider value={{
      navItemClicked: closeSideMenu,
      disableSearch: disableSearch,
      enableSearch: enableSearch,
      searchIconClick: searchIconClickHandler,
      showSearch: showSearch,
      searchInputRef: searchInputRef
    }}>
      <header>
        <MenuBar sideMenuActive={showSideMenu}
          menuIconClickHandler={toggleSideMenu}>
          {navItems}
        </MenuBar>
        <SideMenu isActive={showSideMenu}
          toggleHandler={toggleSideMenu}>
          {navItems}
        </SideMenu>
      </header>
    </MenuContext.Provider>
  );
}

export default NavMenu;