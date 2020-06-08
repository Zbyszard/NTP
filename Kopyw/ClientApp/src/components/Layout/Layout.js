import React, { Component } from 'react';
import NavMenu  from './Menus/NavMenu';

class Layout extends Component {
  static displayName = Layout.name;

  render () {
    return (
      <>
        <NavMenu />
        {this.props.children}
        
      </>
    );
  }
}

export default Layout;