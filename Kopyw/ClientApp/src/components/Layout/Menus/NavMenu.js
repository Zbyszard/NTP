import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { LoginMenu } from '../../api-authorization/LoginMenu';
import MenuBar from './MenuBar/MenuBar';
import SideMenu from './SideMenu/SideMenu';
import NavItem from './NavItem';

class NavMenu extends Component {
  static displayName = NavMenu.name;

  constructor(props) {
    super(props);
    this.state = {
      showMenu: false
    };

    let navItems = [{ url: "/", name: "New" },
    { url: "/top", name: "Top" },
    { url: "/me", name: "Your profile" },
    { url: "/messages", name: "Messages"}]
      .map((item, i) => {
        return (
          <NavItem key={i}>
            <Link to={item.url}>{item.name}</Link>
          </NavItem>);
      });
    this.navItems = [...navItems, <LoginMenu key="login"/>];
  }

  render() {
    return (
      <header>
        <MenuBar sideMenuActive={this.state.showMenu}>
          {this.navItems}
        </MenuBar>
        <SideMenu isActive={this.state.showMenu}>
          {this.navItems}
        </SideMenu>
      </header>
    );
  }
}

export default NavMenu;