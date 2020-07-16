import axios from 'axios';

export const follow = authorId => {
    return axios.post("/follow/add", { authorId: authorId });
}

export const unfollow = authorId => {
    return axios.delete(`/follow/delete/${authorId}`);
}