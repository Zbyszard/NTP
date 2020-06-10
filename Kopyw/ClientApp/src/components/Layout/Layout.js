import React, { Component } from 'react';
import NavMenu from './Menus/NavMenu';
import PageContainer from './PageContainer';

class Layout extends Component {
  static displayName = Layout.name;

  render() {
    return (
      <>
        <NavMenu />
        <PageContainer>
          {this.props.children}
        </PageContainer>
      </>
    );
  }
}

export default Layout;