import React, { Component } from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout/Layout';
import ApiAuthorizationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import { ApplicationPaths } from './components/api-authorization/ApiAuthorizationConstants';
import PostContainer from './components/Posts/PostList/PostList';
import './App.css';

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <Layout>
        <Route path={ApplicationPaths.ApiAuthorizationPrefix} component={ApiAuthorizationRoutes} />
        <Route path="/new" render={props => <PostContainer {...props} getUrl=""/>} />
      </Layout>
    );
  }
}
