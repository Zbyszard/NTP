import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Post from './Post/Post';
import classes from './PostList.module.css';

const tempPosts = [{
    Id: 1,
    AuthorName: "Wasili",
    Title: "Tytul1",
    Text: "Lorem ipsum al;sdf;klsadmflkdsfmv;ldv;ldks;lkfgdsf;lgk;ldfkg;ldfmg;ldfmkg;ldfk;ldkfg;lkdf;lg;dlfkg;lkdfg",
    Score: 13,
    CommentCount: 6,
    UserVote: false
},
{
    Id: 2,
    AuthorName: "Andrzej",
    Title: "Qlsdg;ldkfg[;skd;lgsd;k;lsdfjg;lsdkf;lsdlf;dslnglkskd'lffksjldkf'sdfkj",
    Text: `Lorem ipsum qqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqq
    aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
    qqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqq
    aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
    qqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqq
    aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
    aaaaaaaaaaaaaaaaa`,
    Score: 13,
    CommentCount: 6,
    UserVote: true
},
{
    Id: 3,
    AuthorName: "Sułtan kosmitów",
    Title: "CAZAMATI",
    Text: "Lorem ipsum al;sdf;klsadmflkdsfmv;ldv;ldks;lkfgdsf;lgk;ldfkg;ldfmg;ldfmkg;ldfk;ldkfg;lkdf;lg;dlfkg;lkdfg",
    Score: 13,
    CommentCount: 6,
    UserVote: false
},
{
    Id: 4,
    AuthorName: "Wojtolo Sei'roka",
    Title: "Tytul1",
    Text: "Lorem ipsum al;sdf;klsadmflkdsfmv;ldv;ldks;lkfgdsf;lgk;ldfkg;ldfmg;ldfmkg;ldfk;ldkfg;lkdf;lg;dlfkg;lkdfg",
    Score: 21,
    CommentCount: 49,
    UserVote: false
},
{
    Id: 5,
    AuthorName: "Pan Jaweł",
    Title: "Tytul1",
    Text: "Lorem ipsum al;sdf;klsadmflkdsfmv;ldv;ldks;lkfgdsf;lgk;ldfkg;ldfmg;ldfmkg;ldfk;ldkfg;lkdf;lg;dlfkg;lkdfg",
    Score: 37,
    CommentCount: 6,
    UserVote: true
}];

class PostList extends Component {
    constructor(props) {
        super(props);
        this.state = {
            posts: tempPosts,
            isLoading: true
        }
    }

    render() {
        let content;
        content = this.state.posts.map(p =>
            <Post id={p.Id} key={p.Id}
                author={p.AuthorName}
                title={p.Title}
                text={p.Text}
                score={p.Score}
                commentCount={p.CommentCount}
                userVote={p.UserVote} />);
        return (
            <div className={classes.postList}>
                {content}
            </div>
        );
    }
}

PostList.propTypes = {
    getUrl: PropTypes.string.isRequired
}

export default PostList;