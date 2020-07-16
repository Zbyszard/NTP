import React, { Component } from 'react';
import { GetPostApiConstants, GetPageCountApiConstants } from '../../Shared/ApiConstants/ApiConstants';
import PostList from '../../components/Posts/PostList/PostList';
import PageControllerContext from '../../Context/PageControllerContext';

class MainPage extends Component {
    render() {
        return (
            <PageControllerContext.Consumer>
                {context =>
                    <PostList
                        showForm={true}
                        getPostsUrl={`${GetPostApiConstants.new}/${context.postsPerPage}`}
                        getPageCountUrl={`${GetPageCountApiConstants.all}/${context.postsPerPage}`} />
                }
            </PageControllerContext.Consumer>
        );
    }
}

export default MainPage;