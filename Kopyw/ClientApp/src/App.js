import React, { Component } from 'react';
import { Route, Switch } from 'react-router';
import Layout from './components/Layout/Layout';
import ApiAuthorizationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import authService from './components/api-authorization/AuthorizeService'
import { ApplicationPaths } from './components/api-authorization/ApiAuthorizationConstants';
import PostList from './components/Posts/PostList/PostList';
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
    return (
      <AuthContext.Provider value={this.state.authorizationState}>
        <Layout>
          <Switch>
            <Route path={ApplicationPaths.ApiAuthorizationPrefix} component={ApiAuthorizationRoutes} />
            <Route key={5} exact path="/user/:username/:page?" render={props => <PostList {...props} getUrl="/post/user" />} />
            <Route key={1} exact path="/me/:page?" render={props => <AuthorizedRender><PostList {...props} showForm={true} getUrl={`/post/user/${this.state.authorizationState.userName}`} /></AuthorizedRender>} />
            <Route key={2} exact path="/observed/:page?" render={props => <AuthorizedRender><PostList {...props} getUrl="/post/observed" /></AuthorizedRender>} />
            <Route key={4} exact path="/top/:page?" render={props => <PostList {...props} getUrl="/post/score" />} />
            <Route key={3} exact path="/:page?" render={props => <PostList {...props} showForm={true} getUrl="/post/new" />} />
          </Switch>
        </Layout>
      </AuthContext.Provider>
    );
  }
}
