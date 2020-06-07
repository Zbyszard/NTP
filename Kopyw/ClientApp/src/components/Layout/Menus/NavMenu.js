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
      showSideMenu: false
    };

    let navItems = [{ url: "/", name: "New" },
    { url: "/top", name: "Top" },
    { url: "/me", name: "Your profile" },
    { url: "/messages", name: "Messages" }];
    this.initSideMenu(navItems);
    this.initTopMenu(navItems);
  }

  render() {
    return (
      <header>
        <MenuBar sideMenuActive={this.state.showSideMenu}
          menuIconClickHandler={this.toggleSideMenu}>
          {this.topBarMenuItems}
        </MenuBar>
        <SideMenu isActive={this.state.showSideMenu}
          toggleHandler={this.toggleSideMenu}>
          {this.sideMenuItems}
        </SideMenu>
      </header>
    );
  }

  toggleSideMenu = () => {
    this.setState(state => { return { showSideMenu: !state.showSideMenu }; });
  }

  initTopMenu = menuItems => {
    this.topBarMenuItems = menuItems.map((item, i) => (
      <NavItem key={i}>
        <Link to={item.url}>{item.name}</Link>
      </NavItem>)
    );
    this.topBarMenuItems.push(<LoginMenu key="login" />);
  }

  initSideMenu = menuItems => {
    this.sideMenuItems = menuItems.map((item, i) => (
      <NavItem key={i} clickHandler={this.toggleSideMenu}>
        <Link to={item.url}>{item.name}</Link>
      </NavItem>)
    );
    this.sideMenuItems.push(<LoginMenu key="login" />);
  }

}

export default NavMenu;