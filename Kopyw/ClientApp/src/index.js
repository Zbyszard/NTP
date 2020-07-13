import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter } from 'react-router-dom';
import authService from './components/api-authorization/AuthorizeService';
import axios from 'axios';
import App from './App';
import { LoginActions } from './components/api-authorization/ApiAuthorizationConstants';
//import registerServiceWorker from './registerServiceWorker';

axios.defaults.baseURL = "/api";
axios.interceptors.request.use(async reqConfig => {
  const token = await authService.getAccessToken();
  reqConfig.headers.Authorization = `Bearer ${token}`;
  return reqConfig;
}, error => Promise.reject(error));

axios.interceptors.response.use(resp => resp,
  async error => {
    let auth = await authService.isAuthenticated();
    if (auth && error.response && error.response.status === 401) {
      let state = { returnUrl: window.location };
      let status = await authService.signIn(state);
      if (status === LoginActions.LoginFailed)
        window.location.pathname = "/authentication/login";
      else
        return axios.request(error.config);
    }
    return Promise.reject(error);
  });
const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const rootElement = document.getElementById('root');
ReactDOM.render(
  <BrowserRouter basename={baseUrl}>
    <App />
  </BrowserRouter>,
  rootElement);


// Uncomment the line above that imports the registerServiceWorker function
// and the line below to register the generated service worker.
// By default create-react-app includes a service worker to improve the
// performance of the application by caching static assets. This service
// worker can interfere with the Identity UI, so it is
// disabled by default when Identity is being used.
//
//registerServiceWorker();

