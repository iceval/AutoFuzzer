import axios from 'axios';

const getLogAsync = async (logName: string | undefined) => {
  console.log(logName);
  return await axios
    .post(
      process.env.REACT_APP_API + 'log/getLog',
      { filename: logName },
      { headers: { 'Content-Type': 'multipart/form-data' } }
    )
    .then((response) => response.data)
    .catch(function (error) {
      if (error.response) {
        console.log(error.response.data);
        console.log(error.response.status);
        console.log(error.response.headers);
      } else if (error.request) {
        console.log(error.request);
      } else {
        console.log('Error', error.message);
      }
      console.log(error.config);
    });
};

const getLogNamesAsync = async () => {
  return await axios
    .get(process.env.REACT_APP_API + 'log/getLogNames', {
      headers: { 'Content-Type': 'application/json' },
    })
    .then((response) => response.data)
    .catch(function (error) {
      if (error.response) {
        console.log(error.response.data);
        console.log(error.response.status);
        console.log(error.response.headers);
      } else if (error.request) {
        console.log(error.request);
      } else {
        console.log('Error', error.message);
      }
      console.log(error.config);
    });
};

export const logService = {
  getLogAsync,
  getLogNamesAsync,
};
