export interface ProjectFile {
  name: string;
  path: string;
}

export interface AngularProject {
  type: 'angular';
  components: ProjectFile[];
  services: ProjectFile[];
}

export interface DotNetProject {
  type: 'dotnet';
  controllers: ProjectFile[];
  models: ProjectFile[];
  services: ProjectFile[];
}

export type Project = AngularProject | DotNetProject;
