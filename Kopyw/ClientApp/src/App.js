import React, { Component } from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout/Layout';
import ApiAuthorizationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import authService from './components/api-authorization/AuthorizeService'
import { ApplicationPaths } from './components/api-authorization/ApiAuthorizationConstants';
import PostList from './components/Posts/PostList/PostList';
import PostForm from './components/Posts/PostForm';
import AuthContext from './AuthContext';
import AuthorizedRender from './AuthorizedRender';
import './App.css';

export default class App extends Component {
  static displayName = App.name;
  constructor(props) {
    super(props);
    this.state = {
      authorizationState: false
    };
    authService.getUser().then(user => this.setState(
      {
        authorizationState: { authorized: !!user }
      }));
  }


  render() {
    let auth = this.authorized;
    return (
      <AuthContext.Provider value={this.state.authorizationState}>
        <Layout>
          <Route path={ApplicationPaths.ApiAuthorizationPrefix} component={ApiAuthorizationRoutes} />
          <AuthorizedRender>
            <Route path="/new" component={PostForm} />
          </AuthorizedRender>
          <Route path="/new" render={props => <PostList {...props} getUrl="" />} />
        </Layout>
      </AuthContext.Provider>
    );
  }
}
