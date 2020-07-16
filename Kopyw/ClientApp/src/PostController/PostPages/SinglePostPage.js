import React, { Component } from 'react';
import { GetPostApiConstants, GetPageCountApiConstants } from '../../Shared/ApiConstants/ApiConstants';
import PostList from '../../components/Posts/PostList/PostList';

class SinglePostPage extends Component {
    render() {
        return (
            <PostList
                singlePostId={this.props.match.params.id}
                getPostsUrl={GetPostApiConstants.single} />
        );
    }
}

export default SinglePostPage;