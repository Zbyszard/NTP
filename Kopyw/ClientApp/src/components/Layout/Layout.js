import React, { useState, useEffect } from 'react';
import { withRouter } from 'react-router-dom';
import NavMenu from './Menus/NavMenu';
import PageContainer from './PageContainer';
import LayoutContext from '../../Context/LayoutContext';
import css from './Layout.module.css';
import AuthorizedRender from '../api-authorization/AuthorizedRender';
import ConversationController from '../../ConversationController/ConversationController';

const Layout = props => {

  const [messageViewEnabled, setMessageViewEnabled] = useState(false);

  useEffect(() => {
    setMessageViewEnabled(false);
  }, [props.location.pathname]);

  useEffect(() => {
    const body = document.getElementsByTagName("body")[0];
    if (messageViewEnabled)
      body.className = css.noOverflow;
    else
      body.className = "";
  }, [messageViewEnabled]);

  return (
    <LayoutContext.Provider value={{
      messageViewEnabled: messageViewEnabled,
      setMessageViewEnabled: setMessageViewEnabled
    }}>
      <NavMenu />
      <PageContainer mobileHidden={messageViewEnabled}>
        {props.children}
      </PageContainer>
      <AuthorizedRender>
        <ConversationController/>
      </AuthorizedRender>
    </LayoutContext.Provider>
  );

}

export default withRouter(Layout);