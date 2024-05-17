import { FC } from 'react';
import './styles.scss';
import WindowForm from '../WindowForm';
import ContainerTable from '../ContainerTable';
import { containerService } from '../../services/containerService';

interface ContainerManagementProps {
  className: string;
}

const ContainerManagement: FC<ContainerManagementProps> = ({ className }) => {
  const runAllContainers = () => {
    containerService.runAllContainersAsync().then();
  };

  const pauseAllContainers = () => {
    containerService.pauseAllContainersAsync().then();
  };

  const deleteAllContainers = () => {
    containerService.deleteAllContainersAsync().then();
  };

  return (
    <div className={`container-management ${className}`}>
      <WindowForm
        className="container-management__body"
        title="Управление контейнерами"
      >
        <ContainerTable className="container-management__container-table" />
        <div className="container-management__buttons">
          <button
            className="container-management__button"
            onClick={runAllContainers}
          >
            Запустить все контейнеры
          </button>
          <button
            className="container-management__button"
            onClick={pauseAllContainers}
          >
            Остановить все контейнеры
          </button>
          <button
            className="container-management__button"
            onClick={deleteAllContainers}
          >
            Удалить все контейнеры
          </button>
        </div>
      </WindowForm>
    </div>
  );
};

export default ContainerManagement;
