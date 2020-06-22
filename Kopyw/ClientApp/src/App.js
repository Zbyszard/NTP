import React, { Component } from 'react';
import { Route, Switch } from 'react-router';
import Layout from './components/Layout/Layout';
import ApiAuthorizationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import authService from './components/api-authorization/AuthorizeService'
import { ApplicationPaths } from './components/api-authorization/ApiAuthorizationConstants';
import PostList from './components/Posts/PostList/PostList';
import PostForm from './components/Posts/PostForm';
import AuthContext from './components/api-authorization/AuthContext';
import AuthorizedRender from './components/api-authorization/AuthorizedRender';
import './App.css';

export default class App extends Component {
  static displayName = App.name;
  constructor(props) {
    super(props);
    this.state = {
      authorizationState: {
        authorized: false,
        userName: ''
      }
    };
    authService.getUser().then(user => this.setState(
      {
        authorizationState: {
          authorized: !!user,
          userName: user && user.name
        }
      }));
  }


  render() {
    let userRoute;
    return (
      <AuthContext.Provider value={this.state.authorizationState}>
        <Layout>
          <Route path={ApplicationPaths.ApiAuthorizationPrefix} component={ApiAuthorizationRoutes} />
          <AuthorizedRender>
            <Route exact path="/" component={PostForm} />
            <Route key={1} exact path="/me" render={props => <PostList {...props} getUrl={`/post/user/${this.state.authorizationState.userName}`} />} />
            <Route key={2} exact path="/observed" render={props => <PostList {...props} getUrl="/post/observed" />} />
          </AuthorizedRender>
            <Route key={3} exact path="/" render={props => <PostList {...props} getUrl="/post/new" />} />
            <Route key={4} exact path="/top" render={props => <PostList {...props} getUrl="/post/score" />} />
            <Route key={5} exact path="/user/:username" render={props => <PostList {...props} getUrl="/post/user" />} />
        </Layout>
      </AuthContext.Provider>
    );
  }
}
