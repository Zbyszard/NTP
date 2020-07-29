import React, { Component } from 'react';
import { withRouter } from 'react-router-dom';
import NavMenu from './Menus/NavMenu';
import PageContainer from './PageContainer';
import LayoutContext from './LayoutContext';

class Layout extends Component {

  state = {
    messageViewEnabled: false
  }

  setMessageViewEnabled = value => {
    this.setState({ messageViewEnabled: value });
  }

  componentDidUpdate = (prevProps, prevState) => {
    if (prevProps.location.pathname !== this.props.location.pathname)
      this.setMessageViewEnabled(false);
  }

  render() {
    return (
      <LayoutContext.Provider value={{
        messageViewEnabled: this.state.messageViewEnabled,
        setMessageViewEnabled: this.setMessageViewEnabled
      }}>
        <NavMenu />
        <PageContainer mobileHidden={this.state.messageViewEnabled}>
          {this.props.children}
        </PageContainer>
      </LayoutContext.Provider>
    );
  }
}

export default withRouter(Layout);