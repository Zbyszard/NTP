import React, { Component } from 'react';
import Layout from './components/Layout/Layout';
import authService from './components/api-authorization/AuthorizeService'
import AuthContext from './Context/AuthContext';
import './App.css';
import PageController from './PostController/PageController';
import axios from 'axios';

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

  componentDidMount = () => {
    axios.get("/test/checkauth").then(r => console.log(r));
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
