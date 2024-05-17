import axios from 'axios';

const getRunningContainerNamesAsync = async () => {
  return await axios
    .get(process.env.REACT_APP_API + 'container/getRunningContainerNames', {
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

const getPausedContainerNamesAsync = async () => {
  return await axios
    .get(process.env.REACT_APP_API + 'container/getPausedContainerNames', {
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

const runContainerAsync = async (containerName: string) => {
  return await axios
    .post(
      process.env.REACT_APP_API + 'container/runContainer',
      {},
      {
        params: {
          containerName: containerName,
        },
        headers: { 'Content-Type': 'text/plain' },
      }
    )
    .then((response) => response.data);
};

const pauseContainerAsync = async (containerName: string) => {
  return await axios
    .post(
      process.env.REACT_APP_API + 'container/pauseContainer',
      {},
      {
        params: {
          containerName: containerName,
        },
        headers: { 'Content-Type': 'text/plain' },
      }
    )
    .then((response) => response.data);
};

const deleteContainerAsync = async (containerName: string) => {
  return await axios
    .post(
      process.env.REACT_APP_API + 'container/deleteContainer',
      {},
      {
        params: {
          containerName: containerName,
        },
        headers: { 'Content-Type': 'text/plain' },
      }
    )
    .then((response) => response.data);
};

const runAllContainersAsync = async () => {
  return await axios
    .post(process.env.REACT_APP_API + 'container/runAllContainers', {
      headers: { 'Content-Type': 'application/json' },
    })
    .then((response) => response.data);
};

const pauseAllContainersAsync = async () => {
  return await axios
    .post(process.env.REACT_APP_API + 'container/pauseAllContainers', {
      headers: { 'Content-Type': 'application/json' },
    })
    .then((response) => response.data);
};

const deleteAllContainersAsync = async () => {
  return await axios
    .post(process.env.REACT_APP_API + 'container/deleteAllContainers', {
      headers: { 'Content-Type': 'application/json' },
    })
    .then((response) => response.data);
};

export const containerService = {
  getRunningContainerNamesAsync,
  getPausedContainerNamesAsync,
  runContainerAsync,
  pauseContainerAsync,
  deleteContainerAsync,
  runAllContainersAsync,
  pauseAllContainersAsync,
  deleteAllContainersAsync,
};
