// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Protos/dataClient.proto
// </auto-generated>
#pragma warning disable 0414, 1591, 8981, 0612
#region Designer generated code

using grpc = global::Grpc.Core;

namespace ServerApp {
  public static partial class ClientData
  {
    static readonly string __ServiceName = "dataClient.ClientData";

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static void __Helper_SerializeMessage(global::Google.Protobuf.IMessage message, grpc::SerializationContext context)
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (message is global::Google.Protobuf.IBufferMessage)
      {
        context.SetPayloadLength(message.CalculateSize());
        global::Google.Protobuf.MessageExtensions.WriteTo(message, context.GetBufferWriter());
        context.Complete();
        return;
      }
      #endif
      context.Complete(global::Google.Protobuf.MessageExtensions.ToByteArray(message));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static class __Helper_MessageCache<T>
    {
      public static readonly bool IsBufferMessage = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(global::Google.Protobuf.IBufferMessage)).IsAssignableFrom(typeof(T));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static T __Helper_DeserializeMessage<T>(grpc::DeserializationContext context, global::Google.Protobuf.MessageParser<T> parser) where T : global::Google.Protobuf.IMessage<T>
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (__Helper_MessageCache<T>.IsBufferMessage)
      {
        return parser.ParseFrom(context.PayloadAsReadOnlySequence());
      }
      #endif
      return parser.ParseFrom(context.PayloadAsNewBuffer());
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::ServerApp.DataRequest> __Marshaller_dataClient_DataRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::ServerApp.DataRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::ServerApp.DataResponse> __Marshaller_dataClient_DataResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::ServerApp.DataResponse.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::ServerApp.MessHistRequest> __Marshaller_dataClient_MessHistRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::ServerApp.MessHistRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::ServerApp.HistoryMessRespons> __Marshaller_dataClient_HistoryMessRespons = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::ServerApp.HistoryMessRespons.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::ServerApp.DataRequest, global::ServerApp.DataResponse> __Method_DataReturn = new grpc::Method<global::ServerApp.DataRequest, global::ServerApp.DataResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "DataReturn",
        __Marshaller_dataClient_DataRequest,
        __Marshaller_dataClient_DataResponse);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::ServerApp.MessHistRequest, global::ServerApp.HistoryMessRespons> __Method_MessageHistoryData = new grpc::Method<global::ServerApp.MessHistRequest, global::ServerApp.HistoryMessRespons>(
        grpc::MethodType.Unary,
        __ServiceName,
        "MessageHistoryData",
        __Marshaller_dataClient_MessHistRequest,
        __Marshaller_dataClient_HistoryMessRespons);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::ServerApp.DataClientReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of ClientData</summary>
    [grpc::BindServiceMethod(typeof(ClientData), "BindService")]
    public abstract partial class ClientDataBase
    {
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::ServerApp.DataResponse> DataReturn(global::ServerApp.DataRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::ServerApp.HistoryMessRespons> MessageHistoryData(global::ServerApp.MessHistRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static grpc::ServerServiceDefinition BindService(ClientDataBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_DataReturn, serviceImpl.DataReturn)
          .AddMethod(__Method_MessageHistoryData, serviceImpl.MessageHistoryData).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static void BindService(grpc::ServiceBinderBase serviceBinder, ClientDataBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_DataReturn, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::ServerApp.DataRequest, global::ServerApp.DataResponse>(serviceImpl.DataReturn));
      serviceBinder.AddMethod(__Method_MessageHistoryData, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::ServerApp.MessHistRequest, global::ServerApp.HistoryMessRespons>(serviceImpl.MessageHistoryData));
    }

  }
}
#endregion
