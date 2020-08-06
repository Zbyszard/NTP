import React, { Component } from 'react';
import Layout from './components/Layout/Layout';
import authService from './components/api-authorization/AuthorizeService'
import AuthContext from './Context/AuthContext';
import './App.css';
import PageController from './PostController/PageController';
import { withRouter } from 'react-router-dom';

class App extends Component {
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
          <PageController />
        </Layout>
      </AuthContext.Provider>
    );
  }
}

export default withRouter(App);