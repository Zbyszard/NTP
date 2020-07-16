import React, { Component } from 'react';
import { Route, Switch, Redirect, withRouter } from 'react-router-dom';
import { ApplicationPaths } from '../components/api-authorization/ApiAuthorizationConstants';
import ApiAuthorizationRoutes from '../components/api-authorization/ApiAuthorizationRoutes';
import MainPage from './PostPages/MainPage';
import TopPage from './PostPages/TopPage';
import ObservedPage from './PostPages/ObservedPage';
import UserPage from './PostPages/UserPage';
import LoggedUserPage from './PostPages/LoggedUserPage';
import SearchPage from './PostPages/SearchPage';
import PostsPerPageContext from '../Context/PageControllerContext';
import AuthContext from '../Context/AuthContext';
import SinglePostPage from './PostPages/SinglePostPage';

class PageController extends Component {
    state = {
        postsPerPage: 10,
        sort: "time",
        sortOrder: "desc"
    }

    render() {
        return (
            <AuthContext.Consumer>
                {authContext =>
                    <PostsPerPageContext.Provider value={{ ...this.state }}>
                        <Switch>
                            <Route path={ApplicationPaths.ApiAuthorizationPrefix} component={ApiAuthorizationRoutes} />
                            <Route exact path="/search/:phrase/:page?" component={SearchPage} />
                            <Route exact path="/user/:username/:page?" component={UserPage} />
                            <Route exact path="/me/:page?" component={authContext.authorized ? LoggedUserPage : null} />
                            <Route exact path="/observed/:page?" component={authContext.authorized ? ObservedPage : null} />
                            <Route exact path="/top/:page?" component={TopPage} />
                            <Route exact path="/post/:id" component={SinglePostPage} />
                            <Route exact path="/:page?" component={MainPage} />
                            <Redirect to="/" />
                        </Switch>
                    </PostsPerPageContext.Provider >
                }
            </AuthContext.Consumer>

        );
    }
}

export default withRouter(PageController);