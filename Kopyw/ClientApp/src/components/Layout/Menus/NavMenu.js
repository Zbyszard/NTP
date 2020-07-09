import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { LoginMenu } from '../../api-authorization/LoginMenu';
import MenuBar from './MenuBar/MenuBar';
import SideMenu from './SideMenu/SideMenu';
import NavItem from './NavItem';
import AuthorizedRender from '../../api-authorization/AuthorizedRender';
import MenuContext from './MenuContext';

class NavMenu extends Component {
  static displayName = NavMenu.name;

  constructor(props) {
    super(props);
    this.state = {
      showSideMenu: false
    };
  }

  render() {
    const navItems =
      <>
        <NavItem>
          <Link to="/">New posts</Link>
        </NavItem>
        <NavItem>
          <Link to="/top">Top</Link>
        </NavItem>
        <AuthorizedRender>
          { <NavItem>
            <Link to="/observed">Observed</Link>
          </NavItem> }
          <NavItem>
            <Link to="/me">Your posts</Link>
          </NavItem>
        </AuthorizedRender>
        <LoginMenu />
      </>;
    return (
      <MenuContext.Provider value={{ navItemClicked: this.closeSideMenu }}>
        <header>
          <MenuBar sideMenuActive={this.state.showSideMenu}
            menuIconClickHandler={this.toggleSideMenu}>
            {navItems}
          </MenuBar>
          <SideMenu isActive={this.state.showSideMenu}
            toggleHandler={this.toggleSideMenu}>
            {navItems}
          </SideMenu>
        </header>
      </MenuContext.Provider>
    );
  }

  toggleSideMenu = () => {
    this.setState(state => { return { showSideMenu: !state.showSideMenu }; });
  }
  closeSideMenu = () => {
    this.setState(state => { return { showSideMenu: false }; });
  }

}

export default NavMenu;