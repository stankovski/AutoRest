
AUTOREST    = "../../../binaries/net45/AutoRest.exe"
INPUT_PATH  = "../../../../azure-rest-api-specs/arm-%s/%s/swagger/%s.json"
NAMESPACE   = "github.com/Azure/azure-sdk-for-go/arm/%s"
SDK_PATH    = "../../../../go/src/github.com/azure/azure-sdk-for-go/arm/"
OUTPUT_PATH = "#{SDK_PATH}%s"

SERVICES = {
	authorization: {version: "2015-01-01"},
	compute: {version: "2015-06-15"},
	features: {version: "2014-08-01-preview"},
	logic: {version: "2015-02-01-preview"},
	network: {version: "2015-05-01-preview"},
	resources: {version: "2014-04-01-preview"},
	scheduler: {version: "2014-08-01-preview"},
	search: {version: "2015-02-28"},
	storage: {version: "2015-05-01-preview"},
	subscriptions: {version: "2014-04-01-preview"},
#	web: {version: "2015-08-01", swagger: "service"}
}

desc "Generate, format, and build all services"
task :default => 'generate:all'

desc "List the known services"
task :services do
	SERVICES.each do |k, v|
		puts "#{k}"
	end
end

namespace :generate do
	desc "Generate all services"
	task :all do
		SERVICES.keys.each {|service| Rake::Task["generate:#{service}"].execute }
	end

	SERVICES.keys.each do |service|
		desc "Generate the #{service} service"
		task service do
			generate(service)
		end
	end
end

namespace :go do
	namespace :format do
		desc "Format all generated services"
		task :all do
			SERVICES.keys.each {|service| format(service) }
		end

		SERVICES.keys.each do |service|
			desc "Format the #{service} service"
			task service do
				format(service)
			end
		end
	end

	namespace :build do
		desc "Build all generated services"
		task :all do
			SERVICES.keys.each {|service| build(service) }
		end

		SERVICES.keys.each do |service|
			desc "Build the #{service} service"
			task service do
				build(service)
			end
		end
	end
end

def generate(service)
	service = service.to_sym

	version = SERVICES[service][:version]
	swagger = SERVICES[service][:swagger] || service
	input = sprintf(INPUT_PATH, service, version, swagger)
	namespace = sprintf(NAMESPACE, service)
	output = sprintf(OUTPUT_PATH, service)
		
	puts "Generating #{service}.#{version}"
	s = `#{AUTOREST} -AddCredentials -CodeGenerator Go -Header MICROSOFT_APACHE -Input #{input} -Namespace #{namespace} -OutputDirectory #{output} -Modeler Swagger`
	raise "Failed generating #{service}.#{SERVICES[service]}" if s =~ /.*FATAL.*/
	puts s unless s =~ /.*WARNING.*/

	format(service)
	build(service)
end

def format(service)
	service = service.to_sym
	path = SDK_PATH + "#{service}"
	s = `gofmt -w #{path}`
	raise "Formatting #{path} failed:\n#{s}\n" if $?.exitstatus > 0
end

def build(service)
	service = service.to_sym
	path = sprintf(NAMESPACE, service)
	s = `go build #{path}`
	raise "Building #{path} failed:\n#{s}\n" if $?.exitstatus > 0
end
