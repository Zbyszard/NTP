import React, { Component } from 'react';
import NavMenu  from './Menus/NavMenu';

class Layout extends Component {
  static displayName = Layout.name;

  render () {
    return (
      <>
        <NavMenu />
        {this.props.children}
        {/* wyjebać i zrobić swój container
        /*<Container>
          
        </Container>
        */}
      </>
    );
  }
}

export default Layout;