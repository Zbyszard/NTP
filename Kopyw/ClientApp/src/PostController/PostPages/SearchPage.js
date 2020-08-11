import React, { Component } from 'react';
import { GetPostApiConstants, GetPageCountApiConstants } from '../../Shared/ApiConstants/ApiConstants';
import PostList from '../../components/Posts/PostList/PostList';
import PageControllerContext from '../../Context/PageControllerContext';

class SearchPage extends Component {

    componentDidMount() {
        if (!this.props.match.params.phrase)
            this.props.history.replace('/');
    }

    render() {
        let phrase = this.props.match.params.phrase;
        return (
            <PageControllerContext.Consumer>
                {context =>
                    <PostList key={phrase}
                        getPostsUrl={`${GetPostApiConstants.search}/${phrase}/${context.sort}/${context.sortOrder}/${context.postsPerPage}`}
                        getPageCountUrl={`${GetPageCountApiConstants.searched}/${phrase}/${context.postsPerPage}`} />
                }
            </PageControllerContext.Consumer>

        );
    }
}

export default SearchPage;