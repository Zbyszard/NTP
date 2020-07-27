import axios from 'axios';
import { FollowApiConstants } from '../ApiConstants/ApiConstants';

export const follow = authorId => {
    return axios.post(FollowApiConstants.add, { authorId: authorId });
}

export const unfollow = authorId => {
    return axios.delete(`${FollowApiConstants.delete}/${authorId}`);
}