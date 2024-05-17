import { FC, useEffect, useState } from 'react';
import './styles.scss';
import { containerService } from '../../services/containerService';

interface ContainerTableProps {
  className: string;
}

const ContainerTable: FC<ContainerTableProps> = ({ className }) => {
  const [runningContainerNames, setRunningContainerNames] =
    useState<string[]>();
  const [pausedContainerNames, setPausedContainerNames] = useState<string[]>();

  const getContainerNames = () => {
    containerService.getRunningContainerNamesAsync().then((data) => {
      setRunningContainerNames(data);
    });
    containerService.getPausedContainerNamesAsync().then((data) => {
      setPausedContainerNames(data);
    });
  };

  const runContainer = (containerName: string) => {
    containerService.runContainerAsync(containerName);
  };

  const pauseContainer = (containerName: string) => {
    containerService.pauseContainerAsync(containerName);
  };

  const deleteContainer = (containerName: string) => {
    containerService.deleteContainerAsync(containerName);
  };

  useEffect(() => {
    getContainerNames();
  }, []);

  useEffect(() => {
    const intervalID = setInterval(() => {
      getContainerNames();
    }, 3000);

    return () => clearInterval(intervalID);
  }, []);

  return (
    <div className={`container-table ${className}`}>
      <div className="container-table__container container-table__header">
        <div className="container-table__status container-table__title">
          Исп.
        </div>
        <div className="container-table__container-name container-table__title">
          Название контейнера
        </div>
        <div className="container-table__delete-title container-table__title">
          Удалить
        </div>
      </div>
      <div className="container-table__container-list">
        {runningContainerNames?.map((containerName) => (
          <div key={containerName} className="container-table__container">
            <div className="container-table__status">
              <button
                className="container-table__button"
                onClick={() => pauseContainer(containerName)}
              >
                Остановить
              </button>
            </div>
            <div className="container-table__container-name">
              {containerName}
            </div>
            <button
              className="container-table__delete"
              onClick={() => deleteContainer(containerName)}
            >
              Удалить
            </button>
          </div>
        ))}
        {pausedContainerNames?.map((containerName) => (
          <div key={containerName} className="container-table__container">
            <div className="container-table__button">
              <button
                className="container-table__button"
                onClick={() => runContainer(containerName)}
              >
                Запустить
              </button>
            </div>
            <div className="container-table__container-name">
              {containerName}
            </div>
            <button
              className="container-table__button"
              onClick={() => deleteContainer(containerName)}
            >
              Удалить
            </button>
          </div>
        ))}
      </div>
    </div>
  );
};

export default ContainerTable;
