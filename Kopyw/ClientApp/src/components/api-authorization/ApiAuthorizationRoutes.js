import React, { Component, Fragment } from 'react';
import { Route } from 'react-router';
import { Login } from './Login';
import { Logout } from './Logout';
import { ApplicationPaths, LoginActions, LogoutActions } from './ApiAuthorizationConstants';

export default class ApiAuthorizationRoutes extends Component {

  render() {
    return (
      <Fragment>
        <Route path={ApplicationPaths.Login} render={() => this.loginAction(LoginActions.Login)} />
        <Route path={ApplicationPaths.LoginFailed} render={() => this.loginAction(LoginActions.LoginFailed)} />
        <Route path={ApplicationPaths.LoginCallback} render={() => this.loginAction(LoginActions.LoginCallback)} />
        <Route path={ApplicationPaths.Profile} render={() => this.loginAction(LoginActions.Profile)} />
        <Route path={ApplicationPaths.Register} render={() => this.loginAction(LoginActions.Register)} />
        <Route path={ApplicationPaths.LogOut} render={() => this.logoutAction(LogoutActions.Logout)} />
        <Route path={ApplicationPaths.LogOutCallback} render={() => this.logoutAction(LogoutActions.LogoutCallback)} />
        <Route path={ApplicationPaths.LoggedOut} render={() => this.logoutAction(LogoutActions.LoggedOut)} />
      </Fragment>);
  }

  loginAction = name => {
    return (<Login action={name}></Login>);
  }

  logoutAction = name => {
    return (<Logout redirect={this.props.history.push} action={name}></Logout>);
  }
}