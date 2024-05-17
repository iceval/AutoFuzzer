interface ReflectionMethodsProps {
  methodName: string;
  isSelected: boolean;
  dictionaryName: string;
}

interface ReflectionClassesProps {
  className: string;
  reflectionMethods: ReflectionMethodsProps[];
}

export interface RefletionProjectProps {
  projectName: string;
  reflectionClasses: ReflectionClassesProps[];
}
