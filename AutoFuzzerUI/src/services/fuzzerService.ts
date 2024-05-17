import axios from 'axios';
import { RefletionProjectProps } from '../interfaces/Dtos/reflectionProject';

const getReflectionProjectAsync = async (testDllPath: string) => {
  return await axios
    .post(
      process.env.REACT_APP_API + 'fuzzer/getreflectionproject',
      { testDllPath: testDllPath },
      { headers: { 'Content-Type': 'application/json' } }
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

const runAsync = async (
  testDllPath: string,
  dllPath: string,
  reflectionProject: RefletionProjectProps | undefined
) => {
  return await axios
    .post(
      process.env.REACT_APP_API + 'fuzzer/run',
      {
        TestDllPath: testDllPath,
        DllPath: dllPath,
        ReflectionProject: reflectionProject,
      },
      { headers: { 'Content-Type': 'application/json' } }
    )
    .then((response) => response.data);
};

export const fuzzerService = {
  getReflectionProjectAsync,
  runAsync,
};
